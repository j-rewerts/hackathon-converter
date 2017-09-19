export interface IIssue {
  messages: string;
  type: string;
}

export interface IFile {
  name: string;
  status: string;
  issues?: IIssue[];
}
