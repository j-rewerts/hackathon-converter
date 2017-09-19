export interface IIssue {
  messages: string[];
}

export interface IFile {
  name: string;
  status: string;
  issues?: IIssue[];
}
