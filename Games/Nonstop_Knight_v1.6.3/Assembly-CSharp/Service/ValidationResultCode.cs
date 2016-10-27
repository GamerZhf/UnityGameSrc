namespace Service
{
    using System;

    public enum ValidationResultCode
    {
        VALID,
        PACKAGE_NAME_INVALID,
        SIGNATURE_INVALID,
        RECEIPT_NOT_PARSEABLE,
        NO_STORE_CONFIG_FOUND,
        NO_PRODUCT_CONFIG_FOUND,
        DUPLICATE_RECEIPT,
        RECEIPT_INVALID
    }
}

