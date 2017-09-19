import { Component, OnInit } from '@angular/core';
import { IFile } from '../interfaces/report.interface';

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {

  fileList: IFile[];

  constructor() { }

  ngOnInit() {
  }
}
