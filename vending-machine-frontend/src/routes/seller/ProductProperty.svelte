<script>
    import Communicator from '$lib/VendingMachineAPI';
    import { createEventDispatcher } from 'svelte';
	const dispatch = createEventDispatcher();
    export let productId;
    export let name = "dsfghdjgh";
    export let cost= 20;
    export let amountAvailable;
    let editMode = false;
    let newvalue = {
        name,
        cost,
        amountAvailable,
    }
    async function Submit(){


        if(newvalue.name != name){
            let response = await Communicator.product.updateName({productId,name:newvalue.name});
            if(response.status==200){
                name = newvalue.name;
            }
        }


        if(newvalue.cost != cost){
            let response = await Communicator.product.updatePrice({productId,cost:newvalue.cost});
            if(response.status==200){
                cost = newvalue.cost;
            }
        }


        if(newvalue.amountAvailable != amountAvailable){
            let response = await Communicator.product.updateQuantity({productId,amount: newvalue.amountAvailable});
            if(response.status==200){
                amountAvailable = newvalue.amountAvailable;
            }
        }
        editMode = false;
    }
    async function Delete(){
        let response = await Communicator.product.delete(productId);
        if(response.status == 200){
            dispatch('delete');
        }
    }
</script>


<tr>
    <td><input type="text" bind:value={newvalue.name} class="input input-bordered w-full max-w-xs" disabled={!editMode} /></td>
    <td><input type="text" bind:value={newvalue.cost} class="input input-bordered w-full max-w-xs" disabled={!editMode} /></td>
    <td><input type="text" bind:value={newvalue.amountAvailable} class="input input-bordered w-full max-w-xs" disabled={!editMode} /></td>
    {#if !editMode}
    <td>
        <button on:click={()=>{editMode= true}} class="btn">Edit</button>
        <button on:click={Delete} class="btn">Delete</button>
    </td>
    {:else}
    <td>
        <button on:click={Submit} class="btn">Submit</button>
        <button on:click={()=>{editMode= false}} class="btn">Cancel</button>
    </td>
    {/if}
</tr> 