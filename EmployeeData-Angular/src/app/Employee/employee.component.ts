import { Component, OnInit } from '@angular/core';
import { Iemployee } from './employee';
import { EmployeeService } from './employee.service';
import { FormsModule, FormGroup, NgForm } from '@angular/forms';

@Component({
    selector: 'Employee-Data',
    templateUrl: './employee.component.html',
    providers: [EmployeeService]
})

export class EmployeeDataComponent {
    employees: Iemployee;    

    constructor(private _employeeService: EmployeeService) { }

    ngOnInit(): void{
        this.getEmployeeData();
    }

    getEmployeeData(): void {
        this._employeeService.getEmployee()
            .subscribe((employeeData) => this.employees = employeeData);
    }

    deleteEmployee(code: string): void {
        this._employeeService.delete(code)
            .subscribe();
    }
    
}