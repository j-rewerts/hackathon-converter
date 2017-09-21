export interface IIssue {
  id: string;
  message: string;
  type: string;
}

export interface IFile {
  id: string;
  fileName: string;
  status: string;
  issues?: IIssue[];
}
