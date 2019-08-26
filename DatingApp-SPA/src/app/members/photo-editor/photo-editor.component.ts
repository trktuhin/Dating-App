import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() memberMainPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url : this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken : 'Bearer ' + localStorage.getItem('token'),
      isHTML5 : true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo: Photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          isMain: res.isMain,
          description: res.description
        };
        this.photos.push(photo);
      }
    };
  }

  setMainPhoto(photoId: number) {
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photoId).subscribe( () => {
      this.photos.filter(p => p.isMain === true)[0].isMain = false;
      const photo = this.photos.filter(p => p.id === photoId)[0];
      photo.isMain = true;
      this.memberMainPhotoChange.emit(photo.url);
    }, errr => {
      this.alertify.error(errr);
    });
  }

  deletePhoto(photoId: number) {
    this.alertify.confirm('Are you sure to delete this photo', () => {
      this.userService.deletePhoto(this.authService.decodedToken.nameid, photoId).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
        this.alertify.success('Photo has been deleted ');
      }, err => {
        this.alertify.error('Could not delete the photo');
      });
    });
  }

}
