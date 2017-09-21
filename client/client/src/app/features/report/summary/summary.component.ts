import {
  Component,
  OnDestroy,
  ChangeDetectorRef
} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { GoogleApiService, GoogleAuthService } from 'ng-gapi';
import GoogleAuth = gapi.auth2.GoogleAuth;

import { IFile } from '../interfaces/report.interface';
import { IReportConfig, ReportConfig } from 'app/config/report.config';
import { ReportService } from 'app/sevices/report.service';

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})

export class SummaryComponent implements OnDestroy {

  fileList: IFile[];
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
          this.fileList = data;
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
