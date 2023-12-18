import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@enviroments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {
  baseURL = environment.apiURL + 'api/redesSociais'

constructor( private http: HttpClient ) { }

/**
 *
 * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrita em minúsculo.
 * @param id Precisa passar o palestranteId ou o eventoId dependendo da sua origem.
 * @returns Observable<RedeSocial[]>
 */
public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]> {
  let URL =
    id == 0
    ? `${this.baseURL}/${origem}`
    : `${this.baseURL}/${origem}/${id}`

    return this.http.get<RedeSocial[]>(URL).pipe(take(1))
}

/**
 *
 * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrita em minúsculo.
 * @param id Precisa passar o palestranteId ou o eventoId dependendo da sua origem.
 * @param redesSociais Precisa adicionar redes sociais organizadas em RedeSocial[].
 * @returns Observable<RedeSocial[]>
 */
public saveRedesSociais(
  origem: string,
  id: number,
  redesSociais: RedeSocial[]
  ): Observable<RedeSocial[]> {
    let URL =
      id == 0
      ? `${this.baseURL}/${origem}`
      : `${this.baseURL}/${origem}/${id}`

    return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1))
}

/**
 *
 * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrita em minúsculo.
 * @param id Precisa passar o palestranteId ou o eventoId dependendo da sua origem.
 * @param redeSocialId Precisa adicionar o id da rede social.
 * @returns Observable<any> - Pois é o retorno da Rota.
 */
public deleteRedeSocial(
  origem: string,
  id: number,
  redeSocialId: number,
  ): Observable<any> {
    let URL =
      id == 0
      ? `${this.baseURL}/${origem}/${redeSocialId}`
      : `${this.baseURL}/${origem}/${id}/${redeSocialId}`

    return this.http.delete(URL).pipe(take(1))
}

}
