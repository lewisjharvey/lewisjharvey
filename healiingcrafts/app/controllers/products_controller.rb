class ProductsController < ApplicationController
  def index
  end
  
  def new
    @product = Product.new
  end
  
  def list
    @products = Product.find_by_product_type(params[:product_type_id], :order => 'product_name')
  end
  
  def edit
    @product = Product.find(params[:id])
  end
  
  def create
    @product = Product.new(params[:product])

      if @product.save
        flash[:notice] = 'Product was successfully created.'
        redirect_to(product_url)
      else
        render :action => "new"
      end
  end
  
  def update
    @product = Product.find(params[:id])

      if @product.update_attributes(params[:product])
        flash[:notice] = 'Product was successfully updated.'
        redirect_to(product_url)
      else
        render :action => "edit"
      end
  end
  
  def destroy
    @product = Product.find(params[:id])
    @product.destroy

    redirect_to(product_url)
  end
end
