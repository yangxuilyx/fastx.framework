import { environment } from '@env/environment';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import {
  of,
  switchMap,
  Observable,
  throwError as _observableThrow,
  of as _observableOf
} from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';


export abstract class ClientBase {
  transformResult(
    url_: string,
    response_: any,
    processResult: (r: HttpResponseBase) => Observable<any>
  ): Observable<any> {
    return processResult(response_).pipe(
      switchMap(t => {
        return of(t.data);
      })
    );
  }

  getBaseUrl(url: string): string {
    return environment.API_BASE_URL;
  }
}