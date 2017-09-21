import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SummaryComponent } from './features/report/summary/summary.component';
import { PageNotFoundComponent } from './page-not-found.component';

const appRoutes: Routes = [
  // {path: '', redirectTo: '/reports', pathMatch: 'full'},
  {path: '', component: SummaryComponent},
  {path: 'reports', component: SummaryComponent},
  {path: '404', component: PageNotFoundComponent},
  {path: '**', redirectTo: '/404'}];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ],
  providers: []
})
export class AppRoutingModule { }
