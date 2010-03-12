# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_realalemap_session',
  :secret      => '56337770d4e326fe8c582c42bd8835d28227d61b999d65c8c3c93147143c02067d80216f80bbbc65e267c51f4423c9313a288d958afecc1302be39933f676071'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
