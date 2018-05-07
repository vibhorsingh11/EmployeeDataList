import { Injectable } from '@angular/core';
import { Http, Response ,Headers, RequestOptions} from '@angular/http';
import { Iemployee } from './employee';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class EmployeeService {
    private baseUrl = 'http://localhost:4883/';
    result: any;
    constructor(private http: Http) { }

    getEmployee(): Observable<Iemployee>{
        return this.http.get(this.baseUrl + 'api/employeeapi')
            .map((response: Response) => <Iemployee>response.json());
    }      

    delete(code: string): Observable<any> {
        return this.http.delete(this.baseUrl + 'api/employeeapi/' + code)
            //.map((response: Response) => <any>response.json());
            .map(result => this.result = result.json())
    }
}