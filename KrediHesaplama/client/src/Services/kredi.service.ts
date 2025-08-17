import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class KrediService {
  
  // Bu düzeltilmiş kod APIden dönen Observableı geri döndürür
  logHesaplama(hesaplamaData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/hesapla`, hesaplamaData);
  }
  // Backendin temel rotası
  private apiUrl = 'https://localhost:7014/api/kredi';

  constructor(private http: HttpClient) {}

  // Bu metot POST isteği göndererek yeni bir kredi başvurusu yapar
  // Adres olarak temel rotaya '/basvur' endpoint'ini ekler
  basvuruYap(basvuru: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/basvur`, basvuru);
  }

  // Bu metot GET isteği göndererek TCKNye ait başvuruları getirir
  getBasvurular(tckn: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/basvurular/${tckn}`);
  }

  // Bu metot DELETE isteği göndererek bir başvuruyu iptal eder
  // Adres olarak temel rotaya '/iptal/basvuruId' endpoint'ini ekler
  iptalEt(basvuruId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/iptal/${basvuruId}`);
  }

  hesapla(hesaplamaData: { creditTypeId: number, tutar: number, vade: number }): Observable<any> {
    const url = `${this.apiUrl}/hesapla`;
    return this.http.post(url, hesaplamaData);
  }
}
