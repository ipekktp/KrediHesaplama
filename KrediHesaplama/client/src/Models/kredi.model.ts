
export interface KrediBasvuru {
  userId: number; 
  tckn: string;
  krediTuru: KrediTuru;
  tutar: number;
  vade: number;
}


export interface KrediTuru {
  urunAdi: string;
  faizOrani: number;  // Yıllık faiz oranı %
  minTutar: number;
  maxTutar: number;
}