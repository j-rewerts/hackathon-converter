export interface IIssue {
  id: string;
  message: string;
  type: string;
}

export interface IFile {
  id: string;
  filename: string;
  status: string;
  issues?: IIssue[];
}
