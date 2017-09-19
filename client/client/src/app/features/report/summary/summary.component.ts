import { Component, OnInit } from '@angular/core';
import { IFile } from '../interfaces/report.interface';


const FILES: IFile[] = [
  { id: 1, name: 'file 1', status: 'complete' },
  { id: 2, name: 'file 2', status: 'complete', 
    issues: [ { id: 1, message: 'formula in C1 cannot be converted', type: 'Unconvertable Formula' }, 
              { id: 2, message: 'Worksheet test1 contains 7 million cells', type: '> 2M Cells' },
              { id: 3, message: 'Workbook contains VBA macros', type: 'VBA Macros' }
            ] },
  { id: 3, name: 'file 3', status: 'pending' },
  { id: 4, name: 'file 4', status: 'complete', 
  issues: [ { id: 4, message: 'You got problems yo', type: 'Unconvertable Formula' }, 
            { id: 5, message: 'got 99 problems but 3 mill cells aint one', type: '> 2M Cells' },
            { id: 6, message: 'VBA LYFE', type: 'VBA Macros' }
          ] },
];

@Component({
  selector: 'cv-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})




export class SummaryComponent implements OnInit {

  fileList = FILES;
  selectedFile: IFile;
  
  constructor() { }

  ngOnInit() {
  }
  onSelect(file: IFile): void {
    this.selectedFile = file;
  }
}
