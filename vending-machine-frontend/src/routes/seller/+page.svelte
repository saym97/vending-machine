<script>
  import { onDestroy, onMount,setContext } from 'svelte';
  import Communicator from '$lib/VendingMachineAPI';
	import { currentUser } from '$lib/stores';
	import { goto } from '$app/navigation';
	import ProductProperty from './ProductProperty.svelte';
	import CreateProduct from './CreateProduct.svelte';
  let products = [];

    onMount(async ()=>{
        //if($currentUser?.role == 0)
          //goto('/');
       GetProducts();
    })

    async function GetProducts(){
      products = null;
    let response = await Communicator.product.listAll();

    if (response.status == 200) {
      products = await response.json();
    }
  }
</script>

<CreateProduct on:create={GetProducts}/>
<div class="overflow-x-auto">
    <table class="table w-full">
      <!-- head -->
      <thead>
        <tr>
          <th></th>
          <th>Name</th>
          <th>Cost</th>
          <th>Amount Available</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {#each products as product, i }
          <ProductProperty {...product} on:delete={GetProducts}/>
        {/each}
      </tbody>
    </table>
  </div>