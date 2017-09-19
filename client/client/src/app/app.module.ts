import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CovalentLayoutModule, CovalentStepsModule /*, any other modules */ } from '@covalent/core';
// (optional) Additional Covalent Modules imports
import { CovalentHttpModule } from '@covalent/http';
import { CovalentHighlightModule } from '@covalent/highlight';
import { CovalentMarkdownModule } from '@covalent/markdown';
import { CovalentDynamicFormsModule } from '@covalent/dynamic-forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { SummaryComponent } from './features/report/summary/summary.component';
import { IssuesComponent } from './features/report/issues/issues.component';
import { PageNotFoundComponent } from './page-not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    SummaryComponent,
    IssuesComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CovalentLayoutModule,
    CovalentStepsModule,
    // (optional) Additional Covalent Modules imports
    CovalentHttpModule.forRoot(),
    CovalentHighlightModule,
    CovalentMarkdownModule,
    CovalentDynamicFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
