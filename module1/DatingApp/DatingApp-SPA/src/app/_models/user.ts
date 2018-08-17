import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: string;
    gender: string;
    create: string;
    lastActive: string;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string; // optional
    introduction?: string; // optional
    lookingFor?: string; // optional
    photos?: Photo[];
}
