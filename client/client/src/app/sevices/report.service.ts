import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpInterceptorService } from '@covalent/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { API_ENDPOINT } from '../config/app.config';

@Injectable()
export class ReportService {

  constructor(private httpService: HttpInterceptorService) { }

  getReports(): Observable<Response> {
    return this.httpService.get(API_ENDPOINT)
      .map((res: Response) => res.json());
  }
}
