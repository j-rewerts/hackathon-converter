import { IElement } from './element.interface';
import { ILink } from './link.interface';
import { IIcon } from './icon.interface';

export interface ILinkElement extends IElement {
  link: ILink;
  navIcon?: IIcon;
}
