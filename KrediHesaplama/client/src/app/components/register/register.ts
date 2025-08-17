import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';       // <- kesin ekle
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.html',
  imports: [FormsModule, HttpClientModule, RouterModule],
})

export class RegisterComponent {
  tckn = '';
  ad = '';
  soyad = '';
  sifre = '';

  constructor(private http: HttpClient, private router: Router) {}

  onRegister() {
    const newUser = {
      tckn: this.tckn,
      adSoyad: this.ad + ' ' + this.soyad,
      sifre: this.sifre
    };

    this.http.post<any>('https://localhost:7014/api/user/register', newUser).subscribe({
      next: () => {
        alert('Kayıt başarılı!');
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.log(newUser);
        alert('Kayıt başarısız: ' + err.error);
      }
    });
  }
}
