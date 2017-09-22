import { IIcon } from './icon.interface';

export interface IElement {
  name: string;
  type?: string;
  display?: string;
  description?: string;
  tooltip?: string;
  icon?: IIcon;
}
