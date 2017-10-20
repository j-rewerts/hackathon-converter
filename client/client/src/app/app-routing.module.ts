import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SummaryComponent } from './features/report/summary/summary.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { HomeComponent } from './features/home/home.component';
import { ConvertComponent } from './features/report/convert/convert.component';

const appRoutes: Routes = [
  // {path: '', redirectTo: '/reports', pathMatch: 'full'},
  {path: '', component: SummaryComponent},
  {path: 'home', component: HomeComponent},
  {path: 'reports', component: SummaryComponent},
  {path: 'convert', component: ConvertComponent},
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
