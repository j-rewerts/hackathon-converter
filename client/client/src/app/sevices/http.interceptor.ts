import { Injectable } from '@angular/core';
import { RequestOptionsArgs, Response, Headers } from '@angular/http';
import { IHttpInterceptor } from '@covalent/http';

import { STORAGE_TOKEN_KEY } from 'app/config/app.config';

@Injectable()
export class CustomInterceptor implements IHttpInterceptor {

  onRequest(requestOptions: RequestOptionsArgs): RequestOptionsArgs {
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('x-api-key', 'ebff9c3915244999be14db2bf13d3944');
    headers.append('Authorization', `Bearer ${localStorage.getItem(STORAGE_TOKEN_KEY)}`);
    requestOptions.headers = headers;
    return requestOptions;
  }

  onRequestError(requestOptions: RequestOptionsArgs): RequestOptionsArgs {
    return requestOptions;
  }

  onResponse(response: Response): Response {
    return response;
  }

  onResponseError(error: Response): Response {
    return error;
  }
}
