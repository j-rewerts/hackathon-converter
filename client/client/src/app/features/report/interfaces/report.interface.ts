export interface IIssue {
  id: number;
  message: string;
  type: string;
}

export interface IFile {
  id: number;
  fileName: string;
  status: string;
  issues?: IIssue[];
}
