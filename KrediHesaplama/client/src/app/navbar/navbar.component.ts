import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, NgIf],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(private router: Router) {}

  isLoggedIn(): boolean {
    // Kullanıcının oturum açıp açmadığını kontrol eder
    // 'musteriId anahtarının localStorageda olup olmadığına bakar
    return !!localStorage.getItem('musteriId');
  }

  logout() {
    // Kullanıcının oturumunu sonlandırır
    // 'musteriId' anahtarını localStorage'dan kaldırır
    localStorage.removeItem('musteriId');
    // Kullanıcıyı anasayfaya yönlendirir
    this.router.navigate(['/']);
  }
}
