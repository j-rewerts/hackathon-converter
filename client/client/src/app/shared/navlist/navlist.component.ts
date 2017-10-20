import {
  Component,
  OnChanges,
  SimpleChanges,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';

import { IListElement } from '../interfaces/list-element.interface';
import { IListItem } from '../interfaces/list-item';

@Component({
  selector: 'cv-navlist',
  templateUrl: './navlist.component.html',
  styleUrls: ['./navlist.component.scss'],
})

export class NavListComponent implements OnChanges {

  @Input() navListData: IListElement;
  @Input() selectedIndex: number;
  @Input() multiLine: boolean;
  @Input() showDivider: boolean;
  @Input() showNavIcon: boolean;
  @Output() clicked: EventEmitter<string>;
  @Output() selectedIndexChange: EventEmitter<number>;
  selectable: boolean;

  constructor() {
    this.clicked = new EventEmitter<string>();
    this.selectedIndexChange = new EventEmitter<number>();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.hasOwnProperty('selectedIndex')) {
      this.selectable = true;
    }
    if (! changes.hasOwnProperty('multiLine')) {
      this.multiLine = false;
    }
    if (! changes.hasOwnProperty('showDivider')) {
      this.showDivider = false;
    }
    if (! changes.hasOwnProperty('showNavIcon')) {
      this.showNavIcon = false;
    }
  }

  onClicked(selectedIndex: number, selectedItem: IListItem): void {
    if (selectedIndex !== this.selectedIndex) {
      this.selectedIndexChange.emit(selectedIndex);
    }
    this.selectedIndex = selectedIndex;
    this.clicked.emit(selectedItem.name);
  }
}
