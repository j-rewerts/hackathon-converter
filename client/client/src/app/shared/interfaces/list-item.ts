import { IElement } from './element.interface';
import { ILinkElement } from './link-element.interface';
import { IListElement } from './list-element.interface';

export type IListItem = IElement | ILinkElement | IListElement;

