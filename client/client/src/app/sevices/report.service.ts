import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpInterceptorService } from '@covalent/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { API_ENDPOINT } from '../config/app.config';
import { IFile } from '../features/report/interfaces/report.interface';

@Injectable()
export class ReportService {

  constructor(private httpService: HttpInterceptorService) {}

  analyzeFile(fileId: string): Observable<any> {
    // return this.httpService.post(API_ENDPOINT.ANALYZE_FILE + fileId, {})
    //   .map((res: Response) => res);
    return this.httpService.post(API_ENDPOINT.ANALYZE_FILE, {})
      .map((res: Response) => res);
  }

  getReports(): Observable<IFile[]> {
    return this.httpService.get(API_ENDPOINT.GET_REPORTS)
      .map((res: Response) => res.json().tasks);
  }
}
