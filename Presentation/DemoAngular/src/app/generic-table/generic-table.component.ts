//https://medium.com/@hafeezullah2023/creating-a-dynamic-table-in-angular-with-bootstrap-09dfef86e2d3

import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faFilter } from '@fortawesome/free-solid-svg-icons';
import { faBackwardFast } from '@fortawesome/free-solid-svg-icons';
import { faBackwardStep} from '@fortawesome/free-solid-svg-icons';
import { faForwardStep } from '@fortawesome/free-solid-svg-icons';
import { faForwardFast } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule]
})
export class GenericTableComponent {
  @Input() headers: { key: string; displayName: string }[] = [];
  @Input() data: any[] = [];
  @Input() clickableColumns: string[] = [];
  @Output() rowClicked = new EventEmitter<any>();

  searchTerm: string = '';
  sortColumn: string = '';
  sortDirection: boolean = true;
  currentPage: number = 0;
  pageSize: number = 5;
  faFilter = faFilter;
  faBackwardFast = faBackwardFast;
  faBackwardStep = faBackwardStep;
  faForwardStep = faForwardStep;
  faForwardFast = faForwardFast;

  onRowClick(row: any) {
    this.rowClicked.emit(row);
  }

  onSort(column: string) {
    this.sortDirection = this.sortColumn === column ? !this.sortDirection : true;
    this.sortColumn = column;

    this.data.sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];

      if (aValue < bValue) return this.sortDirection ? -1 : 1;
      if (aValue > bValue) return this.sortDirection ? 1 : -1;
      return 0;
    });

    this.currentPage = 0;
  }

  get filteredData() {
    return this.data.filter(item =>
      Object.values(item).some(value =>
        String(value).toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    );
  }

  get paginatedData() {
    const startIndex = this.currentPage * this.pageSize;
    return this.filteredData.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages() {
    return Math.ceil(this.filteredData.length / this.pageSize);
  }

  get pageStart() {
    return this.currentPage == 0
      ? 1
      : this.currentPage * this.pageSize + 1;
  }

  get pageEnd() {
    var pageEnd = (this.currentPage + 1) * this.pageSize;
    return pageEnd > this.totalRecords ? this.totalRecords : (this.currentPage + 1) * this.pageSize;
  }

  get totalRecords() {
    return this.filteredData.length;
  }

  onPageChange(page: number) {
    if (page >= 0 && page < this.totalPages) {
      this.currentPage = page;
    }
  }

  onFirstPage() {
    this.currentPage = 0;
  }

  onLastPage() {
    this.currentPage = this.totalPages -1;
  }
}
