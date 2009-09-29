class ProductTypesController < ApplicationController
  # GET /product_types
  # GET /product_types.xml
  def index
    @product_types = ProductType.find(:all)
  end

  # GET /product_types/new
  # GET /product_types/new.xml
  def new
    @product_type = ProductType.new
  end

  # GET /product_types/1/edit
  def edit
    @product_type = ProductType.find(params[:id])
  end

  # POST /product_types
  # POST /product_types.xml
  def create
    @product_type = ProductType.new(params[:product_type])

      if @product_type.save
        flash[:notice] = 'ProductType was successfully created.'
        redirect_to(product_types_url)
      else
        render :action => "new"
      end
  end

  # PUT /product_types/1
  # PUT /product_types/1.xml
  def update
    @product_type = ProductType.find(params[:id])

      if @product_type.update_attributes(params[:product_type])
        flash[:notice] = 'ProductType was successfully updated.'
        redirect_to(product_types_url)
      else
        render :action => "edit"
      end
  end

  # DELETE /product_types/1
  # DELETE /product_types/1.xml
  def destroy
    @product_type = ProductType.find(params[:id])
    @product_type.destroy

    redirect_to(product_types_url)
  end
end
