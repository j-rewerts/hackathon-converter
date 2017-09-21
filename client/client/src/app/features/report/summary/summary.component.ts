import {
  Component,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef
} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { GoogleApiService, GoogleAuthService } from 'ng-gapi';
import GoogleAuth = gapi.auth2.GoogleAuth;

import { IFile } from '../interfaces/report.interface';
import { IReportConfig, ReportConfig } from 'app/config/report.config';
import { ReportService } from 'app/sevices/report.service';

enum FlowState {
  INITIAL, ANALYZE_DONE, REPORTS_DONE
}

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class SummaryComponent implements OnDestroy {

  fileList: IFile[];
  reportConfig: IReportConfig;
  authSub: Subscription;
  state: FlowState;
  statusCheckTimer: any;

  constructor(private reportService: ReportService,
              private gapiService: GoogleApiService,
              private googleAuth: GoogleAuthService,
              private changeDetectorRef: ChangeDetectorRef) {

    this.reportConfig = new ReportConfig();
    this.state = FlowState.INITIAL;
    this.gapiService.onLoad(() => {
      this.authSub = this.googleAuth.getAuth().subscribe((auth) => this.getToken(auth));
    });

    this.statusCheckTimer = setInterval(() => {
      if (this.state === FlowState.INITIAL) {
      } else if (this.state === FlowState.ANALYZE_DONE) {
        this.getReports();
      } else {
        clearInterval(this.statusCheckTimer);
      }
    }, 500);
  }

  ngOnDestroy(): void {
    this.authSub.unsubscribe();
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
        data => {
          this.state = FlowState.ANALYZE_DONE;
        },
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
        },
        () => { this.state = FlowState.REPORTS_DONE; }
      );
  }
}
