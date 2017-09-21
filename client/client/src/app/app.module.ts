import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Type } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MdCardModule, MdExpansionModule } from '@angular/material';
import { CovalentExpansionPanelModule, CovalentDataTableModule } from '@covalent/core';
import { CovalentHttpModule, IHttpInterceptor } from '@covalent/http';
import { CovalentHighlightModule } from '@covalent/highlight';
import { CovalentMarkdownModule } from '@covalent/markdown';
import { ClientConfig, GoogleApiModule, NG_GAPI_CONFIG } from 'ng-gapi';

import { CustomInterceptor } from './sevices/http.interceptor';
import { ReportService } from './sevices/report.service';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { SummaryComponent } from './features/report/summary/summary.component';
import { IssuesComponent } from './features/report/issues/issues.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { GOOGLE_API } from './config/app.config';

const httpInterceptorProviders: Type<IHttpInterceptor>[] = [
  CustomInterceptor
];

const gapiClientConfig: ClientConfig = {
  clientId: GOOGLE_API.CLIENT_ID,
  discoveryDocs: [],
  scope: GOOGLE_API.SCOPE,
};

@NgModule({
  declarations: [
    AppComponent,
    SummaryComponent,
    IssuesComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MdCardModule,
    MdExpansionModule,
    CovalentExpansionPanelModule,
    CovalentDataTableModule,
    CovalentHttpModule.forRoot(),
    CovalentHighlightModule,
    CovalentMarkdownModule,
    CovalentHttpModule.forRoot({
      interceptors: [{
        interceptor: CustomInterceptor, paths: ['**'],
      }],
    }),
    GoogleApiModule.forRoot({
      provide: NG_GAPI_CONFIG,
      useValue: gapiClientConfig
    }),
  ],
  providers: [ReportService, httpInterceptorProviders],
  bootstrap: [AppComponent]
})
export class AppModule {
}
