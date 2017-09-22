import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Type } from '@angular/core';
import {
  MdCardModule,
  MdExpansionModule,
  MdIconModule,
  MdListModule,
  MdButtonModule,
  MdTooltipModule,
  MdIconRegistry
} from '@angular/material';
import {
  CovalentExpansionPanelModule,
  CovalentDataTableModule,
  CovalentLayoutModule
} from '@covalent/core';
import { CovalentHttpModule, IHttpInterceptor } from '@covalent/http';
import { CovalentHighlightModule } from '@covalent/highlight';
import { CovalentMarkdownModule } from '@covalent/markdown';
import {
  ClientConfig,
  GoogleApiModule,
  NG_GAPI_CONFIG
} from 'ng-gapi';

import { GOOGLE_API } from './config/app.config';
import { CustomInterceptor } from './sevices/http.interceptor';
import { ReportService } from './sevices/report.service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './features/home/home.component';
import { SummaryComponent } from './features/report/summary/summary.component';
import { ConvertComponent } from './features/report/convert/convert.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { IconComponent } from './shared/icon/icon.component';
import { NavListComponent } from './shared/navlist/navlist.component';

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
    PageNotFoundComponent,
    HomeComponent,
    SummaryComponent,
    ConvertComponent,
    IconComponent,
    NavListComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MdCardModule,
    MdExpansionModule,
    MdIconModule,
    MdListModule,
    MdButtonModule,
    MdTooltipModule,
    CovalentExpansionPanelModule,
    CovalentDataTableModule,
    CovalentHttpModule.forRoot(),
    CovalentHighlightModule,
    CovalentMarkdownModule,
    CovalentLayoutModule,
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

  constructor(mdIconRegistry: MdIconRegistry, private domSanitizer: DomSanitizer) {
    mdIconRegistry.registerFontClassAlias('fontawesome', 'fa');
    mdIconRegistry.addSvgIconInNamespace('assets', 'logo',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/logo.svg'));
    mdIconRegistry.addSvgIconInNamespace('assets', 'github',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/github.svg'));
  }
}
