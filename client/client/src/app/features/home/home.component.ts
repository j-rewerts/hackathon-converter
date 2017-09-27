import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'cv-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  redirectInstall() {
    window.open('https://chrome.google.com/webstore/detail/hacksaw/nopnplelmjnfonhfhmhnpdebmbgkcdhb', '_blank');
  }

  redirectShare() {
    window.open('https://twitter.com/intent/tweet?text=Got%20Office%3F%20Check%20out%20%40Hacksaw.%20%23ATBHackathon', '_blank');
  }
}
