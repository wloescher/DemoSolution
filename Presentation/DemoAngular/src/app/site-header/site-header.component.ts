import { Component } from '@angular/core';
import { faCode } from '@fortawesome/free-solid-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

declare var bootbox: any;

@Component({
  selector: 'demo-site-header',
  templateUrl: './site-header.component.html',
  styleUrls: ['./site-header.component.css'],
})
export class SiteHeaderComponent {
  faCode = faCode;
  faSearch = faSearch;

  search() {
    bootbox.alert('TODO: Search Content');
  }
}
