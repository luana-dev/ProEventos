import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@enviroments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  public usuario = {} as UserUpdate;
  imagemURL = '';
  file : File;

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private accountService: AccountService) { }

  ngOnInit(): void {

  }

  public get ehPalestrante(): boolean{
    return this.usuario.funcao == 'Palestrante';
  }

  get f(): any{
    return '';
  }

  public setFormValue(usuario: UserUpdate): void{
    this.usuario = usuario;
    if(this.usuario.imagemURL)
      this.imagemURL = environment.apiURL + `Resources/Perfil/${this.usuario.imagemURL}`
    else
      this.imagemURL = './assets/img/perfil.png'
  }

  onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  uploadImage(): void{
    this.spinner.show();
    this.accountService.postUpload(this.file).subscribe(
      () => {
        this.toastr.success('Sucesso ao carregar a imagem!', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar imagem', 'Error!');
        console.log(error)
      }
    ).add(() => this.spinner.hide());

  }

}
