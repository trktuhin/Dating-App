import { Photo } from './photo';

export interface User {
    id: number;
    knownAs: string;
    username: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
}
