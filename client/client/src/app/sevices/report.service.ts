import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpInterceptorService } from '@covalent/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Subject } from 'rxjs/Subject';

import { API_ENDPOINT } from '../config/app.config';
import { IFile } from '../features/report/interfaces/report.interface';

@Injectable()
export class ReportService {

  analyzeSuccess$: Subject<void>;

  constructor(private httpService: HttpInterceptorService) {
    this.analyzeSuccess$ = new Subject<void>();
  }

  analyzeFile(data: {}): Observable<any> {
    return this.httpService.post(API_ENDPOINT.ANALYZE_FILE, data)
      .map((res: Response) => this.analyzeSuccess$.next());
  }

  getReports(): Observable<IFile[]> {
    return this.httpService.get(API_ENDPOINT.GET_REPORTS)
      .map((res: Response) => [res.json()]);
  }
}
