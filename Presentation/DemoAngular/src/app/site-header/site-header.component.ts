import { Component } from '@angular/core';
import { faAngular } from '@fortawesome/free-brands-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

declare var bootbox: any;

@Component({
  selector: 'demo-site-header',
  templateUrl: './site-header.component.html',
  styleUrls: ['./site-header.component.css'],
})
export class SiteHeaderComponent {
  faAngular = faAngular;
  faSearch = faSearch;

  search() {
    bootbox.alert('TODO: Implement search');
  }
}
