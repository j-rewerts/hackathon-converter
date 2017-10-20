import { ITdDataTableColumn } from '@covalent/core';

export interface IReportConfig {
  nameLabel: string;
  statusLabel: string;
  issuesLabel: string;
  issueColumns: ITdDataTableColumn[];
  noIssueMessage: string;
}

export class ReportConfig implements IReportConfig {
  nameLabel: string = 'File Name';
  statusLabel: string = 'Status';
  issuesLabel: string = 'Issues';
  issueColumns: ITdDataTableColumn[] = [
    {name: 'type', label: 'Type'},
    {name: 'message', label: 'Message'}
  ];
  noIssueMessage: string = 'No Issues';
}
