import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  tckn: string = '';
  sifre: string = '';

  constructor(private router: Router, private httpClient: HttpClient) {}

  login() {
    const loginData = { tckn: this.tckn, sifre: this.sifre };

    this.httpClient.post<any>('https://localhost:7014/api/user/login', loginData).subscribe({
      next: (response) => {
        console.log(response)
        localStorage.setItem('musteriId', response.userId);
        localStorage.setItem('tckn', this.tckn);
        alert('Giriş başarılı!');
        this.router.navigate(['/home']);
      },
      error: (err) => {
        alert('Giriş başarısız: ' + err.error);
      }
    });
  }

      gotoRegister() {
      this.router.navigate(['/register']);
    }
}
