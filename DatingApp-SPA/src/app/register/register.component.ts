import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister=new EventEmitter();

  model:any={};
  constructor(private authService:AuthService) { }

  ngOnInit() {
  }

  register(){
    this.authService.register(this.model).subscribe(()=>{
      console.log('Registration successful');
    },err=>{
      console.log(err);
    })
    //console.log(this.model);
  }

  cancel(){
    this.cancelRegister.emit();
    console.log("Cancelled");
  }

}
