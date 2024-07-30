import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '../employee.model';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  public employeeId: number | null = null;
  selectedFile: File | null = null;
  employeeForm: FormGroup;
  public cargos = [
    { idCargo: 1, nombre: 'Scrum Master' },
    { idCargo: 2, nombre: 'Desarrollador' },
    { idCargo: 3, nombre: 'QA' },
    { idCargo: 4, nombre: 'PO' }
  ];

  constructor(
    private fb: FormBuilder,  // Asegúrate de importar FormBuilder
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.employeeForm = this.fb.group({
      id: [0],
      cedula: [0],
      nombre: [''],
      fotoPath: [''],
      fechaIngreso: [''],
      idCargo: [0]
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.employeeId = +id;
        this.http.get<Employee>(`${this.baseUrl}api/employees/${this.employeeId}`).subscribe(employee => {
          // Convierte la fecha a formato yyyy-mm-dd para el control de fecha
          const formattedDate = employee.fechaIngreso ? this.convertToDateFormat(employee.fechaIngreso.toString()) : '';
          this.employeeForm.patchValue({
            id: employee.id,
            cedula: employee.cedula,
            nombre: employee.nombre,
            fotoPath: employee.fotoPath,
            fechaIngreso: formattedDate,
            idCargo: employee.idCargo
          });
        });
      }
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('id', this.employeeForm.get('id')?.value);
    formData.append('cedula', this.employeeForm.get('cedula')?.value);
    formData.append('nombre', this.employeeForm.get('nombre')?.value);
    formData.append('fechaIngreso', this.convertFromDateFormat(this.employeeForm.get('fechaIngreso')?.value));
    formData.append('idCargo', this.employeeForm.get('idCargo')?.value);

    if (this.selectedFile) {
      formData.append('file', this.selectedFile);
    }

    if (this.employeeId) {
      this.http.put<void>(`${this.baseUrl}api/employees/${this.employeeId}`, formData).subscribe(() => {
        this.router.navigate(['/employees']);
      });
    } else {
      this.http.post<Employee>(`${this.baseUrl}api/employees`, formData).subscribe(() => {
        this.router.navigate(['/employees']);
      });
    }
  }

  convertToDateFormat(dateString: string): string {
    const [day, month, year] = dateString.split('/');
    return `${day}/${month}/${year}`;
  }

  convertFromDateFormat(dateString: string): string {
    const [year, month, day] = dateString.split('-');
    return `${day}/${month}/${year}`;
  }

}
