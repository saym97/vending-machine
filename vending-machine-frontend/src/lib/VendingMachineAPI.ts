import { currentUser } from "./stores";
import { get } from "svelte/store";

const base_url = "https://localhost:7207";
export enum HTTPMethod
{
    CONNECT = 'CONNECT',
    DELETE = 'DELETE',
    GET = 'GET',
    HEAD = 'HEAD',
    OPTIONS = 'OPTIONS',
    PATCH = 'PATCH',
    POST = 'POST',
    PUT = 'PUT',
    TRACE = 'TRACE'
}

type GenericResponse = {
    Success: boolean,
    Data:any
}

export type User = {
    name : string,
    token : string,
    role : number,
    deposit : number
}

async function BetterHttp(endpoint : string, method: HTTPMethod = HTTPMethod.GET, body:string = null, headers:any = {}){
    console.log(headers);
    let response = 
    await fetch(endpoint, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            ...headers
        },
        body
    });

    return response;
}

async function AuthBetterHttp(endpoint : string, method: HTTPMethod = HTTPMethod.GET, body:string = null, headers:any = {}){
    const user = get(currentUser);
    console.log(user?.token);
    return BetterHttp(endpoint,method,body,{'Authorization':`Bearer ${user?.token}`,...headers});
}

export default {
    user:{
        login:async(data)=> BetterHttp(`${base_url}/user/authenticate`,HTTPMethod.POST,JSON.stringify(data)),
        register:async(data)=> BetterHttp(`${base_url}/user/register`,HTTPMethod.POST,JSON.stringify(data)),
        updatePassword:async(password)=> AuthBetterHttp(`${base_url}/user/update-password`,HTTPMethod.PATCH,JSON.stringify(password)),
        updateusername:async(username)=> AuthBetterHttp(`${base_url}/user/update-username`,HTTPMethod.PATCH,JSON.stringify(username)),
        deposit:async(amount)=> AuthBetterHttp(`${base_url}/user/deposit`,HTTPMethod.PATCH,JSON.stringify(amount)),
        reset:async()=> AuthBetterHttp(`${base_url}/user/reset`,HTTPMethod.PATCH)
    },
    product:{
        listAll: async () => AuthBetterHttp(`${base_url}/product`),
        create: async (data) => AuthBetterHttp(`${base_url}/product/create`,HTTPMethod.POST,JSON.stringify(data)),
        buy : async (data) => AuthBetterHttp(`${base_url}/product/buy`,HTTPMethod.POST,JSON.stringify(data)),
        updateName : async (data) => AuthBetterHttp(`${base_url}/product/update-name`,HTTPMethod.PATCH,JSON.stringify(data)),
        updatePrice : async (data) => AuthBetterHttp(`${base_url}/product/update-price`,HTTPMethod.PATCH,JSON.stringify(data)),
        updateQuantity : async (data) => AuthBetterHttp(`${base_url}/product/update-quantity`,HTTPMethod.PATCH,JSON.stringify(data)),
        delete : async (id) => AuthBetterHttp(`${base_url}/product/delete`,HTTPMethod.DELETE,JSON.stringify(id)),
    }
}