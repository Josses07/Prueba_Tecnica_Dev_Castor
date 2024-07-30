import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../employee.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesListComponent implements OnInit {
  public employees: Employee[] = [];
  public cargos = [
    { idCargo: 1, nombre: 'Scrum Master' },
    { idCargo: 2, nombre: 'Desarrollador' },
    { idCargo: 3, nombre: 'QA' },
    { idCargo: 4, nombre: 'PO' }
  ];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees() {
    this.http.get<Employee[]>(this.baseUrl + 'api/employees').subscribe(result => {
      this.employees = result.map(employee => {
        if (employee.fotoPath) {
          employee.fotoPath = `assets/${employee.fotoPath}`.replace(/\/+/g, '/');
        }
        return employee;
      });
    }, error => console.error(error));
  }

  getCargoName(idCargo: number): string {
    const cargo = this.cargos.find(c => c.idCargo === idCargo);
    return cargo ? cargo.nombre : 'Unknown';
  }

  editEmployee(id: number) {
    this.router.navigate(['/employee-form', id]);
  }

  deleteEmployee(id: number) {
    this.http.delete<void>(`${this.baseUrl}api/employees/${id}`).subscribe(() => {
      this.loadEmployees();
    }, error => console.error(error));
  }

  goToAddEmployee() {
    this.router.navigate(['/employee-form']);
  }

}
