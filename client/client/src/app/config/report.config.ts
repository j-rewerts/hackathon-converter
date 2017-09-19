import { ITdDataTableColumn } from '@covalent/core';

export interface IReportConfig {
  nameLabel: string;
  statusLabel: string;
  issuesLabel: string;
  issueColumns: ITdDataTableColumn[];
}

export class ReportConfig implements IReportConfig {
  nameLabel: string = 'Name';
  statusLabel: string = 'Status';
  issuesLabel: string = 'Issues';
  issueColumns: ITdDataTableColumn[] = [
    {name: 'type', label: 'Type'},
    {name: 'message', label: 'Message'}
  ];
}
