import {
  Component,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef
} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { GoogleApiService, GoogleAuthService } from 'ng-gapi';
import GoogleAuth = gapi.auth2.GoogleAuth;

import { IFile, IIssue } from '../interfaces/report.interface';
import { IReportConfig, ReportConfig } from 'app/config/report.config';
import { ReportService } from 'app/sevices/report.service';

const FILES: IFile[] = [
  { id: '1', fileName: 'file 1', status: 'complete' },
  { id: '2', fileName: 'file 2', status: 'complete',
    issues: [ { id: '1', message: 'formula in C1 cannot be converted', type: 'Unconvertable Formula' },
              { id: '2', message: 'Worksheet test1 contains 7 million cells', type: '> 2M Cells' },
              { id: '3', message: 'Workbook contains VBA macros', type: 'VBA Macros' }
            ] },
  { id: '3', fileName: 'file 3', status: 'pending' },
  { id: '4', fileName: 'file 4', status: 'complete',
  issues: [ { id: '4', message: 'You got problems yo', type: 'Unconvertable Formula' },
            { id: '5', message: 'got 99 problems but 3 mill cells aint one', type: '> 2M Cells' },
            { id: '6', message: 'VBA LYFE', type: 'VBA Macros' }
          ] },
];

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class SummaryComponent implements OnDestroy {

  // fileList: IFile[] = FILES;
  fileList: IFile[];
  // issueMap: IIssue[][];
  selectedFile: IFile;
  reportConfig: IReportConfig;
  authSub: Subscription;
  reportSub: Subscription;

  constructor(private reportService: ReportService,
              private gapiService: GoogleApiService,
              private googleAuth: GoogleAuthService,
              private changeDetectorRef: ChangeDetectorRef) {

    this.reportConfig = new ReportConfig();
    this.gapiService.onLoad(() => {
      this.authSub = this.googleAuth.getAuth().subscribe((auth) => this.getToken(auth));
    });

    this.reportSub = this.reportService.analyzeSuccess$.subscribe(() => this.getReports());
  }

  ngOnDestroy(): void {
    this.authSub.unsubscribe();
    this.reportSub.unsubscribe();
  }

  getToken(auth: GoogleAuth): void {
    auth.signIn()
      .then(res => this.analyzeFile(res.getAuthResponse().access_token))
      .catch((err) => {
        console.log('getToken FAILED ' + err);
      });
  }

  analyzeFile(token: string): void {
    this.reportService.analyzeFile({})
      .subscribe(
        data => {},
        err => {
          console.log('getReports FAILED' + err);
        }
      );
  }

  getReports(): void {
    this.reportService.getReports()
      .subscribe(
        data => {
          // this.fileList = FILES;
          // this.fileList = this.fileList.concat(data);
          this.fileList = data;
          // this.issueMap = [];
          // this.fileList.forEach((file, index) => {
          //   this.issueMap[index] = file.issues;
          // })
          // console.log(FILES);
          // console.log(data);
          this.changeDetectorRef.detectChanges();
        },
        err => {
          console.log('getReports FAILED' + err);
        }
      );
  }

  onSelect(file: IFile): void {
    this.selectedFile = file;
  }
}
