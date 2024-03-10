import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { EmployeeService } from './employee.service';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit, AfterViewInit {

  employees: any[];
  @ViewChild('pieChart') private pieChartRef: ElementRef;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe(data => {
      // Order employees by total time worked
      this.employees = data.sort((a, b) => b.TotalTimeWorked - a.TotalTimeWorked);
    });
  }

  ngAfterViewInit(): void {
    this.drawPieChart();
  }

  drawPieChart(): void {
    const pieChartElement: HTMLCanvasElement = this.pieChartRef.nativeElement;
    const labels = this.employees.map(employee => employee.Name);
    const data = this.employees.map(employee => employee.TotalTimeWorked);
    
    new Chart(pieChartElement, {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'red', 'blue', 'green', 'yellow', 'orange' // Add more colors if needed
          ]
        }]
      }
    });
  }
}
