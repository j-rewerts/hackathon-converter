import { IElement } from './element.interface';
import { IListItem } from './list-item';

export interface IListElement extends IElement {
  placeHolder?: string;
  list: IListItem[];
  navIconClass?: string;
}
