import { Component, OnInit } from '@angular/core';
import { IFile } from '../interfaces/report.interface';

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})


const FILES: IFile[] = [
  { id: 1, name: "file 1", status: "complete" },
  { id: 2, name: "file 2", status: "complete", 
    issues: [ { id: 1, message: "formula in C1 can't be converted", type: "Unconvertable Formula" } ] },
  { id: 3, name: "file 3", status: "pending" }
]


export class SummaryComponent implements OnInit {

  fileList = FILES;
  selectedReport: IFile;
  

  constructor() { }

  ngOnInit() {
  }
}
